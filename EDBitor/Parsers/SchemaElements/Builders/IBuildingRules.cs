namespace EDBitor.Parsers.SchemaElements.Builders
{
    /// <summary>
    /// Encapsulate building restrictions.
    /// Should throw exception is operation is unavailable.
    /// </summary>
    interface IBuildingRules
    {
        void IsAvailableBuildHeader(SchemaElementBuilder builder);
        void IsAvailableBuildElement(SchemaElementBuilder builder);
        void IsAvailableBuildArray(SchemaElementBuilder builder);
        void IsAvailableBuildComment(SchemaElementBuilder builder);
        void IsAvailableBuildConstant(SchemaElementBuilder builder);
    }
}
