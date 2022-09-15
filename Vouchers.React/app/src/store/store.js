import { configureStore } from '@reduxjs/toolkit'
import { userReducer } from './userReducer.js'
import { sidebarReducer } from './sidebarReducer.js'
import { domainAccountReducer } from './domainAccountReducer.js'

const reducer = {
    user: userReducer,
    sidebar: sidebarReducer,
    domainAccount: domainAccountReducer
}
  
export const store = configureStore({
    reducer: reducer,
    middleware: (getDefaultMiddleware) => getDefaultMiddleware({
      serializableCheck: false
    }),
})