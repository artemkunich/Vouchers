import React, {useState, useCallback } from 'react'
import { Header } from '../components/common/header.tsx'
import { NewDomainOffersList } from '../components/newDomain/list.js'

export const PagesNewDomain = () => {

    return (
        <>
            <Header>New domain</Header>
            <NewDomainOffersList/>
        </>
    );
}