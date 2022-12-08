import {createAction, createReducer } from '@reduxjs/toolkit'
import { DomainAccountDto } from '../types/dtos' 

export const domainAccountSelected = createAction('domainAccount/selected') 

interface DomainAccountState {
  currentAccount : DomainAccountDto | undefined,
  currentAccountSet : Boolean
}

const initialdomainAccountState : DomainAccountState = { 
    currentAccount : undefined,
    currentAccountSet : false
}

export const domainAccountReducer = createReducer(
  initialdomainAccountState,
  {
    [ 'domainAccount/selected'] : (state, action) => {
      state.currentAccount = action.payload
      state.currentAccountSet = true
    }
  },
)

