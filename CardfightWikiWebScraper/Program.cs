// See https://aka.ms/new-console-template for more information

using CFVanguard.Data;
using HtmlAgilityPack;
using System.Text.RegularExpressions;


const string cardCSV = @".\cards.csv";
const string artsCSV = @".\arts.csv";
const string urlList = @".\urlsList.txt";
string[] urls = File.ReadAllLines(urlList);

List<Task<string>> webscrapeTasks = new List<Task<string>>();
List<Task<string>> artscrapeTasks = new List<Task<string>>();
List<Task<bool>> downloadscrapeTasks = new List<Task<bool>>();
List<string> cardTable = new List<string>();
List<string> artTable = new List<string>();

// Iterate with speeds of 100
for (int i = 0; i < urls.Length; i += 80)
{
    webscrapeTasks.Clear();
    artscrapeTasks.Clear();
    downloadscrapeTasks.Clear();
    cardTable.Clear();
    artTable.Clear();

    int take = i + 80 < urls.Length ? 80 : urls.Length - i;
    foreach (string fullUrl in urls.Skip(i).Take(take))
    {
        // Read HTML
        webscrapeTasks.Add(ReadDetailsAsync(fullUrl));
    }

    Task.WaitAll(webscrapeTasks.ToArray());
    File.AppendAllLines(cardCSV, cardTable);

    Task.WaitAll(artscrapeTasks.ToArray());
    Console.WriteLine(string.Join("\n", artTable));
    File.AppendAllLines(artsCSV, artTable);

    Task.WaitAll(downloadscrapeTasks.ToArray());
    Console.WriteLine("All Files Downloaded Successfully");
}

async Task<bool> DownloadFilesAsync(Uri imageUri, string fileLocation)
{
    if (File.Exists(fileLocation))
    {
        return true;
    }

    try
    {
        var directoryPath = Path.GetDirectoryName(fileLocation);
        if (directoryPath != null && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
    catch (Exception e)
    {
        string message = e.Message;
    }

    HttpClient client = new HttpClient();
    File.WriteAllBytes(fileLocation, await client.GetByteArrayAsync(imageUri));
    client.Dispose();

    return true;
}

async Task<string> GetCardArtsAsync(string id, string[] cardsetCodes, Uri galleryPage)
{
    try
    {
        HttpClient client = new HttpClient();
        var html = await client.GetStringAsync(galleryPage);

        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var galleries = htmlDoc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("id", "").Contains("gallery-"));
        var enGallery = galleries.FirstOrDefault();
        if (enGallery == null)
        {
            return "";
        }
        var jpGallery = galleries.ElementAtOrDefault(1);

        var enGalleryItems = enGallery.Descendants("div").Where(node => node.GetAttributeValue("class", "") == "wikia-gallery-item");
        var jpGalleryItems = jpGallery != null ? jpGallery.Descendants("div").Where(node => node.GetAttributeValue("class", "") == "wikia-gallery-item") : null;

        string returnString = "";

        foreach (string code in cardsetCodes)
        {
            var galleryNode = enGalleryItems.FirstOrDefault(node => node.InnerText.Contains(code));
            HtmlNode? imageNode = null;

            if (galleryNode != null)
            {
                imageNode = galleryNode.Descendants("a").First(node => node.GetAttributeValue("class", "").Contains("image")).Descendants("img").FirstOrDefault();
            }

            if ((imageNode == null || galleryNode == null) && jpGalleryItems != null)
            {
                galleryNode = jpGalleryItems.FirstOrDefault(node => node.InnerText.Contains(code));

                if (galleryNode == null)
                {
                    continue;
                }
                imageNode = galleryNode.Descendants("a").First(node => node.GetAttributeValue("class", "") == "image lightbox").Descendants("img").FirstOrDefault();
            }


            if (imageNode == null)
            {
                continue;
            }
            var imageUrl = imageNode.GetAttributeValue("src", "");
            imageUrl = imageUrl.Substring(0, imageUrl.LastIndexOf("/scale-to-width-down"));
            var imageUri = new Uri(galleryPage, imageUrl);
            var codeSplit = code.Split('/');
            if (codeSplit.Length == 1)
            {
                Console.Write("");
            }
            var fileLocation = @".\images\" + codeSplit[0];
            fileLocation += @"\" + code.Replace('/', '-') + imageUrl.Substring(imageUrl.LastIndexOf('.'), 4);
            if (fileLocation.Last() == '/') { fileLocation = fileLocation.Substring(0, fileLocation.Count() - 2); }

            var downloadTask = DownloadFilesAsync(imageUri, fileLocation);
            downloadscrapeTasks.Add(downloadTask);
            if (downloadTask.Status == TaskStatus.Created)
            {
                downloadTask.Start();
            }

            //var art = new CFArt(code, fileLocation, "", "");

            artTable.Add("\"" + id + "\"," + code + "," + fileLocation + "," + imageUri);
            returnString += id + "," + code + "," + fileLocation + "\n";
        }

        return returnString;
    }
    catch (Exception e)
    {
        string value = string.Empty;
        string message = e.Message;
        return value;
    }
}

async Task<string> ReadDetailsAsync(string fullUrl)
{
    try
    {
        HttpClient client = new HttpClient();
        var uri = new Uri(fullUrl);
        string html = await client.GetStringAsync(uri);

        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        foreach (HtmlNode brNode in htmlDoc.DocumentNode.SelectNodes("//br"))
        {
            brNode.ParentNode.ReplaceChild(htmlDoc.CreateTextNode("\n"), brNode);
        }

        // Page Name
        string id = htmlDoc.DocumentNode.Descendants("h1")
            .First(node => node.GetAttributeValue("class", "") == "page-header__title").InnerText.Trim();

        // Card Name
        string name = htmlDoc.DocumentNode.Descendants("div").First(node => node.GetAttributeValue("class", "") == "header").InnerText.Split('\n')[0].Trim();

        // Effect Text
        HtmlNode? effectNode = htmlDoc.DocumentNode.Descendants("table").FirstOrDefault(node => node.GetAttributeValue("class", "") == "effect");
        HtmlNode? effectCellNode = effectNode != null ? effectNode.Descendants("td").Last() : null;
        string effect = "";
        if (effectCellNode != null)
        {
            foreach (HtmlNode brNode in effectCellNode.SelectNodes("//hr"))
            {
                brNode.ParentNode.ReplaceChild(htmlDoc.CreateTextNode("---------------------------------------------------------------"), brNode);
            }

            effect = "\"" + effectCellNode.InnerText.Trim().Replace("\n", "\\n").Replace("\"", "\"\"") + "\"";
        }

        // General Card Info
        CFType CFType = CFType.NormalUnit;
        string? subType = null;
        string? trigger = null;
        int grade = 0;
        string? ability = null;
        string? rideskill = null;
        int? power = null;
        int? shield = null;
        string[] nations = new string[0] { };
        string[] clans = new string[0] { };
        string[] races = new string[0] { };
        Format format = Format.Premium;

        //
        HtmlNode infoNode = htmlDoc.DocumentNode.Descendants("div").First(node => node.GetAttributeValue("class", "") == "info-main");
        foreach (HtmlNode rowNode in infoNode.Descendants("tr"))
        {
            var cellNodes = rowNode.Descendants("td");
            string header = cellNodes.First().InnerText.Trim();
            string value = cellNodes.Last().InnerText.Trim();

            switch (header)
            {
                case "Card Type":
                    var typeValues = value.Split('/');
                    switch (typeValues[0])
                    {
                        case "Normal Unit":
                            CFType = CFType.NormalUnit;
                            break;
                        case "Trigger Unit":
                            CFType = CFType.TriggerUnit;
                            break;
                        case "G Unit":
                            CFType = CFType.GUnit;
                            break;
                        case "Normal Order":
                            CFType = CFType.NormalOrder;
                            break;
                        case "Blitz Order":
                            CFType = CFType.BlitzOrder;
                            break;
                        case "Set Order":
                            CFType = CFType.SetOrder;
                            break;
                        case "Trigger Order":
                            CFType = CFType.TriggerOrder;
                            break;
                    }
                    subType = typeValues.Length == 2 ? typeValues[1] : null;
                    break;
                case "Grade / Skill":
                    var gradeSkillValues = value.Split('/');
                    grade = int.Parse(gradeSkillValues[0].Trim().Last().ToString());
                    if (gradeSkillValues.Length == 2)
                    {
                        ability = gradeSkillValues[1].Trim();
                    }
                    break;
                case "Power":
                    try
                    {
                        power = int.Parse(value.Replace("+", ""));
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                    }
                    break;
                case "Shield":
                    try
                    {
                        shield = int.Parse(value);
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                    }
                    break;
                case "Nation":
                    nations = value.Split('/').Select(nation => nation.Trim()).ToArray();
                    break;
                case "Clan":
                    clans = value.Split('/').Select(clan => clan.Trim()).ToArray();
                    break;
                case "Race":
                    races = value.Split('/').Select(race => race.Trim()).ToArray();
                    break;
                case "Format":
                    var formats = value.Split('/').Select(format => format.Trim());
                    if (formats.Count() > 1)
                    {
                        if (formats.First() == "Standard")
                        {
                            format = Format.DStandard | Format.Premium;
                        }
                        else if (formats.First() == "Premium")
                        {
                            format = Format.VPremium | Format.Premium;
                        }
                    }
                    break;
                case "Limitation Text":
                    return "";
                case "Trigger Effect":
                    trigger = value.Split('/')[0];
                    break;
                case "Imaginary Gift":
                    var giftURl = cellNodes.Last().Descendants("a").First().GetAttributeValue("href", null);
                    switch (giftURl)
                    {
                        case string when giftURl.Contains("Force"):
                            rideskill = "Force";
                            break;
                        case string when giftURl.Contains("Protect"):
                            rideskill = "Protect";
                            break;
                        case string when giftURl.Contains("Accel"):
                            rideskill = "Accel";
                            break;
                    }
                    break;
                case "Ride Skill":
                    rideskill = value;
                    break;
            }
        }

        if (rideskill == null) { return ""; }

        var cardsetText = htmlDoc.DocumentNode.Descendants("div").First(node => node.GetAttributeValue("class", "") == "info-extra").InnerText;
        var cardsetMatches = Regex.Matches(cardsetText, "([DGV]-){0,1}([A-Z]{1,3}[0-9]+|PR|MB)/([A-Z]{0,3}){0,1}[0-9]+(?=\\s)");
        var cardsetCodes = cardsetMatches.Select(match => match.Value).ToArray();

        if (cardsetCodes.Length == 0)
        {
            return "";
        }

        string returnString = "\"" + id + "\",\"" + name + "\"," + (int)CFType + "," + subType + "," + trigger + "," + rideskill + "," + grade + "," + ability + "," + power + "," + shield + "," + string.Join('/', nations) + "," + string.Join('/', clans) + "," + string.Join('/', races) + "," + effect + "," + (int)format;
        Console.WriteLine(returnString);

        /*
        var galleryPageNode = htmlDoc.DocumentNode.Descendants("table").First(node => node.GetAttributeValue("class", "") == "misc").Descendants("b").First().Descendants("a").FirstOrDefault();
        var galleryPageURL = galleryPageNode != null ? galleryPageNode.GetAttributeValue("href", "") : "";

        if (galleryPageURL == "")
        {
            var tabberNode = htmlDoc.DocumentNode.Descendants("div").First(node => node.GetAttributeValue("class", "").Contains("cftable")).Descendants("div").First(node => node.GetAttributeValue("class", "") == "");
            var imageNode = tabberNode.Descendants("img").First();

            var imageUrl = imageNode.GetAttributeValue("src", "");
            imageUrl = imageUrl.Substring(0, imageUrl.LastIndexOf("/scale-to-width-down"));
            var imageUri = new Uri(uri, imageUrl);

            string newCode = Regex.Match(imageNode.GetAttributeValue("alt", ""), "([DGV]-){0,1}([A-Z]{1,3}[0-9]+|PR|MB)-([A-Z]{0,3}){0,1}[0-9]+").Value;
            int lastHypen = newCode.LastIndexOf('-');
            if (lastHypen < 0)
            {
                newCode = newCode.Substring(0, lastHypen);
            }
            newCode = newCode.Remove(lastHypen, 1).Insert(lastHypen, "/");

            var fileLocation = @".\images\" + newCode.Split('/')[0] + @"\" + newCode.Replace('/', '-') + imageUrl.Substring(imageUrl.LastIndexOf('.'), 4);

            var downloadTask = DownloadFilesAsync(imageUri, fileLocation);
            downloadscrapeTasks.Add(downloadTask);
            if (downloadTask.Status == TaskStatus.Created)
            {
                downloadTask.Start();
            }

            artTable.Add("\"" + id + "\"," + newCode + "," + fileLocation + "," + imageUri);
        }
        else
        {
            var galleryPageFullURL = new Uri(uri, galleryPageURL);
            artscrapeTasks.Add(GetCardArtsAsync(id, cardsetCodes, galleryPageFullURL));
        }
        */

        cardTable.Add(returnString);
        return returnString;
    }
    catch (Exception e)
    {
        string value = "";
        string message = e.Message;
        return value;
    }
}