# API Documentation: [Project Name]

*Synthesised from `src/[Name].Api/Controllers/` and `.artifacts/architecture/`*
*Last updated: [YYYY-MM-DD]*

---

## Base URL

```
http://localhost:[port]/api
```

---

## Authentication

```txt
[Auth method — JWT Bearer / API Key / None]
Header: Authorization: Bearer <token>
```

---

## Response Format

All responses use the envelope pattern:

```json
{
  "data": { ... },
  "isSuccess": true,
  "error": null
}
```

Error response:

```json
{
  "data": null,
  "isSuccess": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human readable message"
  }
}
```

---

## Endpoints

### [Entity Name]

#### `GET /api/[entity]`

Returns a list of [entities].

**Response:**
```json
[
  {
    "id": "uuid",
    "[field]": "[value]"
  }
]
```

---

#### `GET /api/[entity]/{id}`

Returns a single [entity] by ID.

**Parameters:**
| Name | Type | Required | Description |
|---|---|---|---|
| `id` | `uuid` | Yes | Entity identifier |

**Response:** `200 OK` or `404 Not Found`

---

#### `POST /api/[entity]`

Creates a new [entity].

**Request body:**
```json
{
  "[field]": "[value]"
}
```

**Response:** `201 Created` with created entity, or `400 Bad Request` with validation errors.

---

## Error Codes

*Source: `[Name].Domain/Errors/`*

| Code | HTTP Status | Meaning |
|---|---|---|
| `[ENTITY].NOT_FOUND` | 404 | Entity does not exist |
| `[ENTITY].VALIDATION_ERROR` | 400 | Request failed validation |

---

<!-- TODO: verify all endpoints against actual controllers -->
