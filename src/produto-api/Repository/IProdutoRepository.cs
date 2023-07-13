namespace produto_api.Repository;
public interface IProdutoRepository
{
    public JsonElement BuscarProdutos();
    public string BuscarProdutoPorId(string uuid);
}
