# DGS by Team Utopia

[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](https://forthebadge.com)

**Dijon Go Server est une application web qui permet aux passionnés de Go de se retrouver et de jouer des parties en ligne.**

DGS propose les fonctionnalités suivantes :

- Parties personnalisés avec création d'un salon de jeu public ou privé
  
- Partie en ligne avec matchmaking
  
- Classements
  
- Voir les joueurs connectés et discuter avec eux dans un Chat
  
- Rediffusion des parties
  

## Fabriqué avec

**Front-end** : HTML/CSS/Angular

**Back-end** : ASP.NET Core 

**Databases** : Redis / SQLite

## Comment faire tourner l'application DGS en local ?  

**Prérequis : Vous devez avoir NodeJs installé sur votre machine : https://nodejs.org/fr**

**Prérequis : Vous devez avoir Docker installé sur votre machine : https://www.docker.com/products/docker-desktop/**

Pour enregistrer les coups en temps réel, DGS utilise une base de données Redis.
Redis est une base de données en mémoire rapide et légère. 

1) Exécutez la commande suivante pour télécharger l'image Redis et créer un conteneur nommé `redis-server` :

```bash
docker run --name redis-server -d -p 6379:6379 redis
```

2) Vérifier que le conteneur est actif

Pour vérifier que Redis est en cours d'exécution, utilisez la commande suivante :

```bash
docker ps
```
---

3) Tester Redis avec `redis-cli`

Connectez-vous à Redis via le client intégré `redis-cli` :

   ```bash
  docker exec -it redis-server redis-cli
  ```
Testez la connexion au serveur Redis en envoyant une commande PING :
```bash
PING
```
Le serveur doit répondre avec PONG

---

4) Cloner le repository GitHub dans un dossier spécifique

---

5) Ouvrez la solution Server.sln avec Visual Studio 2022

---

6) Définir plusieurs projets de démarrage et cliquez sur "Démarrer" :
   
   ![image](https://github.com/user-attachments/assets/5c8b88e1-3c87-4f57-93fa-7702662ec1b0)
 
   ![image](https://github.com/user-attachments/assets/fab6f130-3e8a-4476-ac9b-ed7b3cb2ca88)

---
9) Ouvrez ensuite un terminal et déplacez-vous de le répértoire "Client"
Saisissez la commande ```npm install``` (en cas d'erreur saisissez ```npm install --force```)
Enfin lancez le serveur angular : ```npm start```

---

Félicitations ! Le client, l'API et le websocket sont désormais exécution, vous pouvez parcourir et utiliser le site à votre guise.

Note : Pour jouer une partie de Go en local, il vous faudra deux fenêtres avec deux comptes différents sur chacune des fenêtres, l'un des clients doit créer la partie, l'autre doit la rejoindre
   
:warning: L'utilisation de Microsoft Edge est à éviter en local, elle peut entraîner des problèmes de certificats invalides avec l'API.

## Captures d'écrans
![image](https://github.com/user-attachments/assets/b4639287-d222-4030-a35d-f165975056b5)
![image](https://github.com/user-attachments/assets/3ead29d9-6572-49e8-a93f-a77cc8169795)
![image](https://github.com/user-attachments/assets/fe2aae9d-d976-413f-8b04-3bd06fb7f8ef)
![image](https://github.com/user-attachments/assets/fc5d85a0-3fd5-49bc-8b88-ccf369394629)
![image](https://github.com/user-attachments/assets/49fee3b5-70ba-407e-aa11-76caf0804ea5)
![image](https://github.com/user-attachments/assets/dddb3e59-d354-4b0c-ae52-8b963b3f2b7b)
![image](https://github.com/user-attachments/assets/fc36d6e4-fbad-4dad-8d17-75d818a9f6d3)




## Auteurs

* **Victor Duboz** _alias_ [@VictorinhoOo](https://github.com/victorinhoOo)
* **Clément Boutet** _alias_ [@clementbtt](https://github.com/ClementBoutet)
* **Mattis Galopin** _alias_ [@mattis_glp](https://github.com/MattisGaloppin)
* **Adam Stitou** _alias_ [@Dadam](https://github.com/AdamStitou)
* **Louis Deméocq** _alias_ [@KeTeR](https://github.com/0KeTeR0)
