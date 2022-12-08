import React, {useState, useCallback } from 'react'
import { Header } from '../components/header/header.tsx'
import { NewDomainOffersList } from '../newDomain/newDomainOffersList.js'

export const NewDomainContainer = () => {

    return (
        <>
            <Header>New domain</Header>
            <NewDomainOffersList/>
        </>
    );
}