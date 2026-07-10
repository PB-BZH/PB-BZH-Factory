# Checklist de publication PB BZH Concept

## Avant publication

- Vérifier le nom du produit.
- Vérifier la version affichée.
- Vérifier le numéro de version Android si APK.
- Vérifier le keystore utilisé si APK.
- Vérifier le certificat de signature si MSI.
- Nettoyer bin et obj pour les projets Android si nécessaire.

## Publication

- Générer l'artefact MSI ou APK.
- Copier l'artefact dans le dossier web local.
- Générer le fichier SHA256.
- Générer update.json si le produit le nécessite.
- Vérifier les chemins publics.
- Uploader vers le site.

## Après publication

- Tester l'URL directe de l'artefact.
- Tester la page download.php.
- Installer depuis le site.
- Vérifier le lancement de l'application.
- Vérifier l'export/import des données pour les applications Android.
- Faire un commit Git.
- Créer un tag de version si publication stable.