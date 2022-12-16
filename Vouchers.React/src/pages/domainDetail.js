import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainDetailContainer } from '../components/domainDetail/container.tsx'
import { Header } from '../components/common/header.tsx'

export const PagesDomainDetail = () => {

    return <>
        <Header>Domain detail</Header>
        <DomainDetailContainer/>
    </>  
}