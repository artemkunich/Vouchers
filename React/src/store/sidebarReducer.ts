import {createAction, createReducer } from '@reduxjs/toolkit'

export const newOfferClicked = createAction('sidebar/NewOfferClicked') 

interface SidebarState {
  isOffersActive : boolean,
  isNewOfferActive : boolean,
}

const initialSidebarState : SidebarState = { 
    isOffersActive : false,
    isNewOfferActive : false,
}

export const sidebarReducer = createReducer(
  initialSidebarState,
  {
    ['sidebar/OffersClicked'] : (state) => {
      state.isOffersActive = !state.isOffersActive
    }
  },
)

