using Server.Model.DTO;

namespace Server.Model.Data
{
    /// <summary>
    /// Définit un standard pour les classes qui gèrent les parties en base de données.
    /// </summary>
    public interface IGameDAO
    {
        /// <summary>
        /// Récupère la liste des parties disponibles 
        /// </summary>
        /// <returns>Liste de parties</returns>
        public List<AvailableGameInfoDTO> GetAvailableGames();
    }
}
