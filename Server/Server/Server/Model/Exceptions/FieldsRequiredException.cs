using System;

/// <summary>
/// Exception personnalisée qui est déclenchée lorsque tous les champs requis ne sont pas remplis.
/// </summary>
public class FieldsRequiredException : ArgumentException
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FieldsRequiredException"/> avec un message d'erreur par défaut.
    /// </summary>
    public FieldsRequiredException()
        : base("Tous les champs doivent être remplis.")
    {
    }
}
