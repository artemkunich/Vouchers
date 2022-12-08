import { configureStore } from '@reduxjs/toolkit'
import { userReducer } from './userReducer'
import { sidebarReducer } from './sidebarReducer'
import { domainAccountReducer } from './domainAccountReducer'

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

export type RootState = ReturnType<typeof store.getState>