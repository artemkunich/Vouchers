import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainDetail } from '../domainDetail/domainDetail.js'
import { Header } from '../components/header/header.tsx'

export const DomainDetailContainer = () => {

    return <>
        <Header>Domain detail</Header>
        <DomainDetail/>
    </>  
}