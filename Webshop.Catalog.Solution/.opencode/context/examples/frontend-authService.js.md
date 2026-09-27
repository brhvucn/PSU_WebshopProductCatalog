---
domain: frontend
capabilities:
  - examples
keywords:
  - auth service
  - authentication
  - login
priority: medium
cost: low
---

import endpoints from './endpoints'
import apiService from './apiService'

export default {
  async login(username, password) {
    const url = endpoints.AUTH.LOGIN()
    const data = {
      email: username,
      password: password,
    }

    return await apiService.post(url, data)
  },

  async getUsers() {
    return await apiService.get(endpoints.AUTH.USERS())
  },

  async getUser(userid) {
    return await apiService.get(endpoints.AUTH.USER(userid))
  },

  async changepassword(email, currentpassword, newpassword) {
    const url = endpoints.AUTH.CHANGEPASSWORD()
    const data = {
      email: email,
      currentPassword: currentpassword,
      newPassword: newpassword,
    }

    return await apiService.post(url, data)
  },

  async refresh(refreshToken) {
    const url = endpoints.AUTH.REFRESH()
    const data = { refreshToken }
    return await apiService.post(url, data)
  },

  async logout() {
    // Hvis du senere får backend revoke endpoint:
    // return await apiService.post(endpoints.AUTH.LOGOUT(), {})
    return true
  },

  async me() {
    return await apiService.get(endpoints.AUTH.ME())
  },

  async register(username, password) {
    const payload = {
      email: username,
      password: password,
    }

    return await apiService.post(endpoints.AUTH.REGISTER(), payload)
  },
}
