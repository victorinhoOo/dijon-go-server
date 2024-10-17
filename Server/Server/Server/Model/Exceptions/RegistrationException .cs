using System;

/// <summary>
/// Exception déclenchée lorsqu'une erreur survient durant l'inscription d'un utilisateur.
/// </summary>
public class RegistrationException : Exception
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="RegistrationException"/> avec un message d'erreur personnalisé.
    /// </summary>
    /// <param name="message">Le message qui décrit l'erreur.</param>
    public RegistrationException(string message)
        : base("Erreur lors de l'inscription : " + message)
    {
    }
}
