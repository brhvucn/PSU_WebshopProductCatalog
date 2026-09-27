---
domain: frontend
capabilities:
  - examples
keywords:
  - api service
  - axios
  - http client
priority: medium
cost: low
---

import axios from 'axios'
import endpoints from './endpoints'
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'

let api, refreshPromise
const $auth = () => { useAuthStore().logout(); router.push('/login'); throw Error() }

function getApi() {
  if (!api) {
    api = axios.create({ baseURL: endpoints.BASEURL, headers: { 'Content-Type': 'application/json' } })
    api.interceptors.request.use(c => {
      const t = localStorage.getItem('auth_token')
      if (t) c.headers.Authorization = `Bearer ${t}`
      return c
    })
    api.interceptors.response.use(
      r => r,
      async e => {
        const req = e.config, s = e.response?.status
        if (s !== 401 || !req) return Promise.reject(e.response?.data || e)
        if (req.url?.includes('/refresh')) $auth()
        if (req._retry) $auth()
        req._retry = true
        try {
          const a = useAuthStore()
          if (!a.refreshToken) $auth()
          if (!refreshPromise) refreshPromise = a.refresh().catch($auth).finally(() => { refreshPromise = null })
          await refreshPromise
          const t = localStorage.getItem('auth_token')
          if (t) req.headers.Authorization = `Bearer ${t}`
          return api(req)
        } catch (x) { return Promise.reject(x.response?.data || x) }
      },
    )
  }
  return api
}

export default {
  async get(u, p) { return (await getApi().get(u, { params: p || {} })).data },
  async post(u, d) { return (await getApi().post(u, d)).data },
  async put(u, d) { return (await getApi().put(u, d)).data },
  async del(u) { return (await getApi().delete(u)).data },
  async getBlob(u) { return (await getApi().get(u, { responseType: 'blob' })).data },
  async uploadFile(f) {
    const fd = new FormData()
    fd.append('file', f)
    return (await getApi().post(endpoints.FILES.UPLOADFILE(), fd, { headers: { 'Content-Type': 'multipart/form-data' } })).data
  },
}
