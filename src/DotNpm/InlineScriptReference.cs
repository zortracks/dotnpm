namespace DotNpm {

    internal class InlineScriptReference : IScriptReference {

        public InlineScriptReference(string inline) {
            Inline = inline;
        }

        public string Inline { get; }

        public string GetScriptReference() => Inline;
    }
}