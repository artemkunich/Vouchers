import {createAction, createReducer } from '@reduxjs/toolkit'

export const userSigned = createAction('user/signed')
export const identityDefined = createAction('identity/defined')

const initialUserState = { 
    token : undefined,
    identityId : undefined,
    identityDetail : undefined,
    identityIdSet : false
}

export const userReducer = createReducer(
  initialUserState,
  {
    [userSigned] : (state, action) => {
      state.token = action.payload
    },
    [identityDefined] : (state, action) => {
      state.identityId = action.payload?.identityId
      state.identityIdSet = true
      state.identityDetail = action.payload
    }
  },
)

