[Serializable]
public class ProdutoNaoValidoException : Exception
{
    public ProdutoNaoValidoException() { }
    public ProdutoNaoValidoException(string message) : base(message) { }
    public ProdutoNaoValidoException(string message, Exception inner) : base(message, inner) { }
    protected ProdutoNaoValidoException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}