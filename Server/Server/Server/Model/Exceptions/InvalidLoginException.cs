using System;

/// <summary>
/// Exception personnalisée qui est déclenchée lorsqu'une tentative de connexion échoue.
/// </summary>
public class InvalidLoginException : Exception
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="InvalidLoginException"/> avec un message d'erreur par défaut.
    /// </summary>
    public InvalidLoginException()
        : base("L'utilisateur ou le mot de passe est incorrect")
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="InvalidLoginException"/> avec un message d'erreur personnalisé.
    /// </summary>
    /// <param name="message">Le message qui décrit l'erreur.</param>
    public InvalidLoginException(string message)
        : base(message)
    {
    }
}
