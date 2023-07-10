namespace produto_api.database;
public interface IProdutoRepository
{
    public IEnumerable<ProdutoModel> BuscarProdutos();
    public ProdutoModel BuscarProdutosPorId(string uuid);
}
