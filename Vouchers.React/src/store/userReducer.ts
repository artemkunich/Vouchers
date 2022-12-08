import { createAction, createReducer } from '@reduxjs/toolkit'
import { User } from 'oidc-client'

export const userSigned = createAction('user/signed')
export const identityDefined = createAction('identity/defined')

interface UserState {
  token? : User,
  identityId? : string,
  identityDetail? : any,
  identityIdSet : boolean
}

const initialUserState: UserState = { 
    token : undefined,
    identityId : undefined,
    identityDetail : undefined,
    identityIdSet : false
}

export const userReducer = createReducer(
  initialUserState,
  {
    ['user/signed'] : (state, action) => {
      state.token = action.payload
    },
    ['identity/defined'] : (state, action) => {
      state.identityId = action.payload?.identityId
      state.identityIdSet = true
      state.identityDetail = action.payload
    }
  },
)

