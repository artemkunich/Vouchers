import React, {useState, useEffect, useRef, useCallback } from 'react'
import { useSelector} from 'react-redux'
import { IdentityDetailContainer } from '../components/identityDetail/container.js'
import { Header } from '../components/common/header.tsx'

export const PageIdentityDetail = () => {

    const identityId = useSelector((state) => state.user.identityId)
    
    return <>   
        <Header>{identityId ? 'My profile' : 'Please register'}</Header>
        <IdentityDetailContainer/>
    </>  
}