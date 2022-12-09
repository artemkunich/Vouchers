import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainDetail } from '../domainDetail/domainDetail.tsx'
import { Header } from '../components/header/header.tsx'

export const DomainDetailContainer = () => {

    return <>
        <Header>Domain detail</Header>
        <DomainDetail/>
    </>  
}