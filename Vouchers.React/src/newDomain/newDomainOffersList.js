import React, {useState, useEffect, useRef, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { NewDomainOffersListElement } from './newDomainOffersListElement.js'
import { FetchingListItemsInfo} from '../components/fetchingListItemsInfo/fetchingListItemsInfo.tsx'
import { useLoadOnScroll } from '../api/helpers.ts'
import { api } from '../api/api.ts'

import "bootstrap-icons/font/bootstrap-icons.css";

export const NewDomainOffersList = () => {
    
    const token = useSelector(state => state.user.token)

    const [domainOffers, setDomainOffers] = useState([]);
    const [domainOffersFilter, setDomainOffersFilter] = useState({
        name: "",
        pageSize: 10
    });

    const getDomainOffersAsync = useCallback(async (query) => await api.getIdentityDomainOffers(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(domainOffersFilter, getDomainOffersAsync, setDomainOffers)

    return (
        <>       
            {           
                domainOffers && domainOffers.map((offer, i) => <NewDomainOffersListElement key={i} offersItem={offer} postDomainConract={async (domainName) => await api.postDomain(token, {offerId: offer.id, domainName: domainName})}/>)
            }
            <FetchingListItemsInfo isFetching={isFetching}/>
        </>
    );
}