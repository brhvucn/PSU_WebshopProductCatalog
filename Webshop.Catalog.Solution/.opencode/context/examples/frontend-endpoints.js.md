---
domain: frontend
capabilities:
  - examples
keywords:
  - endpoints
  - configuration
  - api routes
priority: medium
cost: low
---

// Runtime config (Docker) or fallback for local dev
const cfg = window.__env ?? {
  API_BASE_URL: 'https://localhost:7133',
}

//API_BASE_URL: "https://localhost:7059"

const BASE = cfg.API_BASE_URL // e.g. http://ragservice:8080
const API = `${BASE}/api` // automatic /api prefix
const AUTH = `${API}/auth` // auth controller is at api/auth (login, register, etc.)

export default {
  BASEURL: API,

  // ---- ORGANIZATIONS ----
  ORGANISATIONS: {
    GETALL: () => `${API}/organizations`,
    GETBYID: (id) => `${API}/organizations/${id}`,
    UPDATE: (id) => `${API}/organizations/${id}`,
    CREATE: () => `${API}/organizations`,
    DELETE: (id) => `${API}/organizations/${id}`,
  },

    // ---- USERS ----
  USERS: {
    GETALL: (id) => `${API}/users/organization/${id}`,
    GETBYID: (id) => `${API}/users/${id}`,
    UPDATE: (id) => `${API}/users/${id}`,
    CREATE: () => `${API}/users`,
    DELETE: (id) => `${API}/users/${id}`,
    ADDTOORGANIZATION: (id) => `${API}/users/addtoorganization/${id}`
  },
}
