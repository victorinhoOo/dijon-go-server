namespace Server.Model.Exceptions
{
    using System;

    /// <summary>
    /// Exception personnalisée déclenchée lorsqu'un utilisateur tente de s'inscrire avec un nom d'utilisateur déjà utilisé.
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="UserAlreadyExistsException"/> 
        /// avec un message d'erreur par défaut, indiquant que le nom d'utilisateur est déjà pris.
        /// </summary>
        public UserAlreadyExistsException()
            : base("Ce nom d'utilisateur existe déjà.")
        {
        }
    }
}
