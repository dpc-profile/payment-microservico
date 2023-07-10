namespace produto_api.Repository;

public class ProdutoRepository : IProdutoRepository
{
    public string BuscarProdutoPorId(string uuid)
    {
        ArrayEnumerator json = LoadData().EnumerateArray();

        JsonElement resultado = json.FirstOrDefault(
            x => x.GetProperty(propertyName: "uuid").GetString() == uuid);


        if (resultado.ValueKind is JsonValueKind.Undefined)
        {
            throw new KeyNotFoundException(message: "Produto não encontrado");
        }

        return resultado.GetRawText();
    }

    public JsonElement BuscarProdutos()
    {
        return LoadData();
    }

    public JsonElement LoadData()
    {
        string conteudoJson = File.ReadAllText("./Repository/products.json");

        // Fazer o parsing do JSON para um objeto
        var jsonDocument = JsonDocument.Parse(conteudoJson);

        // Acessar a propriedade "products"
        JsonElement productsElement = jsonDocument.RootElement.GetProperty("products");

        return productsElement;
    }
}