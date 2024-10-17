namespace Server.Model.Exceptions
{
    using System;

    /// <summary>
    /// Exception personnalisée qui est déclenchée lorsqu'une erreur survient 
    /// lors de la mise à jour des informations d'un utilisateur.
    /// </summary>
    public class UpdateUserException : Exception
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="UpdateUserException"/> 
        /// avec un message d'erreur personnalisé.
        /// </summary>
        /// <param name="message">Le message qui décrit l'erreur spécifique.</param>
        public UpdateUserException(string message)
            : base("Erreur lors de la mise à jour de l'utilisateur : " + message)
        {
        }
    }
}
