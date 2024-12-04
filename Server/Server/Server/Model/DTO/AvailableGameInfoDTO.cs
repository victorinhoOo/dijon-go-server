﻿namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les informations d'une partie qu'il est possible de rejoindre pour le transfert au client
    /// </summary>
    public class AvailableGameInfoDTO
    {
        private int id;
        private string title;
        private int size;
        private string rule;

        /// <summary>
        /// L'id de la partie
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// Le titre de la partie
        /// </summary>
        public string Title { get => title; set => title = value; }

        /// <summary>
        /// Taille de la grille de jeu
        /// </summary>
        public int Size { get => size; set => size = value; }

        /// <summary>
        /// Règles de la partie
        /// </summary>
        public string Rule { get => rule; set => rule = value; }
    }
}
