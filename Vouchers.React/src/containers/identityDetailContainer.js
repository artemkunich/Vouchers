import React, {useState, useEffect, useRef, useCallback } from 'react'
import { useSelector} from 'react-redux'
import { IdentityDetail } from '../identityDetail/identityDetail.js'
import { Header } from '../components/header/header.tsx'

export const IdentityDetailContainer = () => {

    const identityId = useSelector((state) => state.user.identityId)
    
    return <>   
        <Header>{identityId ? 'My profile' : 'Please register'}</Header>
        <IdentityDetail/>
    </>  
}