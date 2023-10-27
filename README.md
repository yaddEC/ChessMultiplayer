# Chess Multiplayer Game

Un jeu d'échecs en ligne qui vous permet de jouer contre un adversaire à travers sur le même réseau.

## Screenshots

**Menu Principal**:
![Menu Principal](Screenshot/1.png)
Ici, vous pouvez choisir de créer un serveur, rejoindre un serveur , jouer contre une IA primitive ou quitter le jeu.

**Création de Serveur**:
![Création de Serveur](Screenshot/2.png)
L'adresse IP est pré-remplie, seul le port doit être choisi.

**Rejoindre une Partie**:
![Rejoindre une Partie](Screenshot/3.png)
Vous devrez entrer l'adresse IP et le port du serveur auquel vous souhaitez vous connecter.

**Gameplay**:
![Gameplay](Screenshot/4.png)
Aperçu du gameplay avec deux instances du jeu en action.

**Déconnexion**:
![Déconnexion](Screenshot/5.png)
Un message s'affiche lorsque votre adversaire se déconnecte.

## À propos du jeu
Ce jeu d'échec est spécial. Vous ne devez pas juste mettre l'adversaire en echec et mat... Vous devez TUER son ROI. Vous devez récupérer LA TETE DU LOUIS!!!

## Techniques utilisées
Ce jeu utilise plusieurs techniques pour gérer les fonctionnalités de réseau:
- **Socket**: Permet la communication bidirectionnelle entre le serveur et le client.
- **Binary Formatter**: Utilisé pour la sérialisation et la désérialisation de données et de classe(des mouvement d'échec dans notre cas), permettant ainsi leur transfert via le réseau.
- **Gestion des Connexions**: Implémentation de méthodes pour vérifier si un client est toujours connecté, et gérer les déconnexions inattendues.

