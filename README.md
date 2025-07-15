# 🧠 Dotnet Semantic Search with Embeddings Demo

This project is a **.NET 9**-based backend solution demonstrating **semantic search** using **AI-generated embeddings** and a **vector database**. It uses **Ollama (Mistral model)** for generating embeddings and **Qdrant** as the vector store.

---

## 🔍 Features

- ✅ AI-powered **semantic product search**
- ✅ Embedding generation using **Ollama** (Mistral model)
- ✅ Vector storage & similarity search with **Qdrant**
- ✅ RESTful **.NET 9 API** to query and manage semantic data
- ✅ **Fully dockerized** with `docker-compose`
- ✅ Ready-to-run with a single command

---

## 🧱 Architecture

```plaintext
+------------------+        +-----------------+       +---------------+
|   Client (e.g.   | <----> |  .NET API       | <-->  |   Qdrant DB   |
| Postman / CURL)  |        |  (Semantic API) |       | (Vector Store)|
+------------------+        +-----------------+       +---------------+
                                   |
                                   |
                          +------------------+
                          |  Ollama (Mistral)|
                          |  Embedding Model |
                          +------------------+

```

---

## 🔗 Services:

- Semantic Search API: http://localhost:5000
- Qdrant Dashboard: http://localhost:6333
- Ollama API: http://localhost:11434

Before sending embedding requests, make sure the Nomic-embed-text model is required to be pulled:

ollama pull nomic-embed-text
