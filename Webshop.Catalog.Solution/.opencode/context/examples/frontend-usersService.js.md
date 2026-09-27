---
domain: frontend
capabilities:
  - examples
keywords:
  - users service
  - crud
  - api
priority: medium
cost: low
---

import endpoints from './endpoints'
import apiService from './apiService'

export default {
  async getUsers(organizationid) {
    const url = endpoints.USERS.GETALL(organizationid)
    const result = await apiService.get(url)
    return result.result
  },

   async getUserById(userid) {
    const url = endpoints.USERS.GETBYID(userid)
    const result = await apiService.get(url)
    return result.result
  },

  async updateUser(payload) {
    const url = endpoints.USERS.UPDATE(payload.id);
    const result = await apiService.put(url, payload);
    return result.result;
  },

  async addUserToOrganization(payload){
     const url = endpoints.USERS.ADDTOORGANIZATION(payload.organizationid);
    const result = await apiService.post(url, payload);
    return result.result;
  }
}
