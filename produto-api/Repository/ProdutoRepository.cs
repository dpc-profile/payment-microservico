namespace produto_api.database;

public class ProdutoRepository : IProdutoRepository
{
    public IEnumerable<ProdutoModel> BuscarProdutos()
    {
        throw new NotImplementedException();
    }

    public ProdutoModel BuscarProdutosPorId(string uuid)
    {
        throw new NotImplementedException();
    }

    public object loadData()
    {
        string allText = System.IO.File.ReadAllText("./products.json");

        object? jsonObject = JsonConvert.DeserializeObject(allText);
        return jsonObject;
    }
}