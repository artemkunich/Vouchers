import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainOffersList } from '../domainOffers/domainOffersList.js'
import { Header } from '../components/header/header.tsx'

export const DomainOffersContainer = () => {

    return <>
        <Header>Domain offers</Header>
        <DomainOffersList/>
    </>
}