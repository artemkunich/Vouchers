import {createAction, createReducer } from '@reduxjs/toolkit'

export const domainAccountSelected = createAction('domainAccount/selected') 

const initialdomainAccountState = { 
    currentAccount : undefined,
    currentAccountSet : false
}

export const domainAccountReducer = createReducer(
  initialdomainAccountState,
  {
    [domainAccountSelected] : (state, action) => {
      state.currentAccount = action.payload
      state.currentAccountSet = true
    }
  },
)

