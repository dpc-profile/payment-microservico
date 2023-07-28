[Serializable]
public class UsuarioNaoValidoException : Exception
{
    public UsuarioNaoValidoException() { }
    public UsuarioNaoValidoException(string message) : base(message) { }
    public UsuarioNaoValidoException(string message, Exception inner) : base(message, inner) { }
    protected UsuarioNaoValidoException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}