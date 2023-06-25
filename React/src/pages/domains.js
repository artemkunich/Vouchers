import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainsList } from '../components/domains/list.js'
import { Header } from '../components/common/header.tsx'

export const PagesDomains = () => {

    return <>
        <Header>Domains</Header>
        <DomainsList/>
    </>
}