# 🧠 Dotnet Semantic Search with Embeddings Demo

This project is a complete **.NET 9**-based semantic search solution using AI-generated **embeddings** and a **vector database**. It demonstrates how to perform advanced **semantic product search** with **Dockerized microservices**, **Qdrant**, **Ollama**, and two different UIs: **HTML** and **Blazor**.

---

## 🔍 Features

- ✅ AI-powered **semantic search** via embedding vectors
- ✅ **Product catalog** search using meaning-based similarity
- ✅ Embedding generation with **Ollama** (Mistral model)
- ✅ Vector storage and similarity search with **Qdrant**
- ✅ **Fully dockerized** with `docker-compose`
- ✅ Ready-to-use development setup with a single command

---

## 🧱 Architecture

```plaintext
+-------------------+        +-----------------+       +---------------+
|   Blazor UI       | <----> |  .NET API       | <-->  |   Qdrant DB   |
|   (Client-side)   |        |  (Semantic API) |       | (Vector Store)|
+-------------------+        +-----------------+       +---------------+
          ^                           ^
          |                           |
+-------------------+        +---------------------------+
|   HTML UI         |        |  Ollama (nomic-embed-text)|
|  (MVC Frontend)   |        |  Embedding Model          |
+-------------------+        +---------------------------+
