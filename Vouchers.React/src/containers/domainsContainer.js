import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainsList } from '../domains/domainsList.js'
import { Header } from '../components/header/header.tsx'

export const DomainsContainer = () => {

    return <>
        <Header>Domains</Header>
        <DomainsList/>
    </>
}