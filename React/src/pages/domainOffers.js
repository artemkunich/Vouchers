import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainOffersList } from '../components/domainOffers/list.js'
import { Header } from '../components/common/header.tsx'

export const PagesDomainOffers = () => {

    return <>
        <Header>Domain offers</Header>
        <DomainOffersList/>
    </>
}