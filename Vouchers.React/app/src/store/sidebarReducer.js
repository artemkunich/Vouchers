import {createAction, createReducer } from '@reduxjs/toolkit'

export const offersClicked = createAction('sidebar/OffersClicked') 
export const newOfferClicked = createAction('sidebar/NewOfferClicked') 

const initialSidebarState = { 
    isOffersActive : false,
    isNewOfferActive : false,
}

export const sidebarReducer = createReducer(
  initialSidebarState,
  {
    [offersClicked] : (state, action) => {
      state.isOffersActive = !state.isOffersActive
    }
  },
)

