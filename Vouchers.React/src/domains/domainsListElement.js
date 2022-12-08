import React, { useState } from 'react'
import { ListElement } from '../components/list/list.tsx'
import { getImageSrc } from '../imageSources.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainsListElement = ({domainsItem, postSubscriptionRequest}) => {

    return <ListElement>

        <div className="col-2 align-middle">
            { domainsItem.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(domainsItem.imageId)}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{domainsItem.name}</p>
            <p>{domainsItem.description}</p>
        </div> 
        <div className=" col-2 d-grid mt-2 mb-2">
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} title={"Subscribe"} onClick={async () => await postSubscriptionRequest(domainsItem.id)}>
                Subscribe
            </button>
        </div>

    </ListElement>
}