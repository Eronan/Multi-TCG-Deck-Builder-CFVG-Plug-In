using CFVanguard.Formats;
using IGamePlugInBase;

namespace CFVanguard
{
    public class CFVanguard : IGamePlugIn
    {

        public string Name => "cfvg";

        public string LongName => "Cardfight Vanguard";

        public byte[] IconImage { get => Properties.Resources.CardBack; }

        public string AboutInformation => throw new NotImplementedException();

        public IEnumerable<IFormat> Formats => new IFormat[3] { new DStandard(), new VPremium(), new Premium() };

        /*
public string Name => "cfvg";

public string LongName => "Cardfight Vanguard";

public byte[] IconImage { get => Properties.Resources.CardBack; }

public IFormat[] Formats { get; private set; }

public bool CardListInitialized => false;

public IImportMenuItem[] ImportFunctions => new IImportMenuItem[0];

public IExportMenuItem[] ExportFunctions => new IExportMenuItem[0];

public string AboutInformation => throw new NotImplementedException();

public SearchField[] SearchFields => throw new NotImplementedException();

public string DownloadLink => throw new NotImplementedException();

public List<DeckBuilderCard> AdvancedFilterSearchList(IEnumerable<DeckBuilderCard> cards, SearchField[] searchFields)
{
    throw new NotImplementedException();
}

public int CompareCards(DeckBuilderCard x, DeckBuilderCard y)
{
    throw new NotImplementedException();
}

public Task DownloadFiles()
{
    throw new NotImplementedException();
}

public void InitializePlugIn()
{

}
*/
    }
}