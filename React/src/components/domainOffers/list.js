import React, { useState, useRef, useEffect, useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { DomainOffersListFilter } from './listFilter.tsx'
import { DomainOffersListNewElement } from './listNewElement.tsx'
import { DomainOffersListElement } from './listElement.tsx'
import { FetchingListItemsInfo } from '../common/fetchingListItemsInfo.tsx'
import { useLoadOnScroll } from '../../api/helpers.ts'
import { api } from '../../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainOffersList = ({handleCancel}) => {
    
    const token = useSelector(state => state.user.token)

    const [domainOffers, setDomainOffers] = useState([]);
    const [domainOffersFilter, setDomainOffersFilter] = useState({});

    const getDomainOffersAsync = useCallback(async (query) => await api.getDomainOffers(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(domainOffersFilter, getDomainOffersAsync, setDomainOffers)

    const postDomainOfferAsync = useCallback(async (domainOffer) => {
        const domainOfferId = await api.postDomainOffer(token, domainOffer)   
        if(domainOfferId) {
            resetPageIndex() 
        }
    }, [api, token, domainOffersFilter])

    const handleSearch = useCallback((filter) => {
        setDomainOffersFilter({...domainOffersFilter, ...filter})
        resetPageIndex()
    }, [])
    
    return (
        <>
            <DomainOffersListFilter handleSearch={handleSearch} handleCancel={handleCancel}/>
            <DomainOffersListNewElement postDomainOffer={postDomainOfferAsync}/>         
            {              
                domainOffers.map((offer, i) => <DomainOffersListElement key={offer.id} offer={offer} putDomainOffer={async (offerValues) => await api.putDomainOffer(token, offerValues)}/>)
            }
            <FetchingListItemsInfo isFetching={isFetching}/>
        </>
    );
}





