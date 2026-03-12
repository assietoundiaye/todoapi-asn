# TodoApi ASN

API REST construite avec ASP.NET Core, MongoDB Atlas, et sécurisée avec JWT.

## Technologies utilisées
- ASP.NET Core 10.0
- MongoDB Atlas
- JWT Authentication
- Docker
- Déployé sur Render.com

## Prérequis
- .NET 10 SDK
- MongoDB Atlas (compte gratuit)
- Docker (optionnel)

## Installation et exécution locale

### 1. Cloner le repo
```bash
git clone https://github.com/VOTRE_USERNAME/todoapi-asn.git
cd todoapi-asn/TodoApi_AsN
```

### 2. Configurer les secrets
```bash
dotnet user-secrets init
dotnet user-secrets set "MongoDb__ConnectionString" "votre_connection_string"
dotnet user-secrets set "MongoDb__DatabaseName" "TodoDb"
dotnet user-secrets set "Jwt__Key" "votre_cle_secrete_min_32_caracteres"
dotnet user-secrets set "Jwt__Issuer" "TodoApi"
dotnet user-secrets set "Jwt__Audience" "TodoApiUsers"
```

### 3. Lancer l'application
```bash
dotnet run
```
L'API sera disponible sur `http://localhost:5007`

## Exécution avec Docker
```bash
docker build -t todoapi-asn .
docker run -p 8080:8080 \
  -e MongoDb__ConnectionString="votre_connection_string" \
  -e MongoDb__DatabaseName="TodoDb" \
  -e Jwt__Key="votre_cle_secrete" \
  -e Jwt__Issuer="TodoApi" \
  -e Jwt__Audience="TodoApiUsers" \
  todoapi-asn
```

## API en production
**URL** : https://todoapi-asn.onrender.com

## Endpoints

| Méthode | Endpoint | Accès | Description |
|---------|----------|-------|-------------|
| POST | /api/auth/register | Public | Créer un compte |
| POST | /api/auth/login | Public | Se connecter |
| GET | /api/TodoItems | Authentifié | Lire tous les todos |
| GET | /api/TodoItems/{id} | Authentifié | Lire un todo |
| POST | /api/TodoItems | Admin | Créer un todo |
| PUT | /api/TodoItems/{id} | Admin | Modifier un todo |
| DELETE | /api/TodoItems/{id} | Admin | Supprimer un todo |

## Exemples d'utilisation

### Créer un compte admin
```bash
curl -X POST https://todoapi-asn.onrender.com/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!","role":"admin"}'
```

### Se connecter
```bash
curl -X POST https://todoapi-asn.onrender.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'
```

### Créer un Todo (admin)
```bash
curl -X POST https://todoapi-asn.onrender.com/api/TodoItems \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer VOTRE_TOKEN" \
  -d '{"name":"Mon todo","isComplete":false}'
```

### Lire les Todos
```bash
curl https://todoapi-asn.onrender.com/api/TodoItems \
  -H "Authorization: Bearer VOTRE_TOKEN"
```
