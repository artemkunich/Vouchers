import React, { useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { domainAccountSelected } from '../store/domainAccountReducer.js';
import { Link } from 'react-router-dom'
import style from './sidebar.module.css'
import "bootstrap-icons/font/bootstrap-icons.css";

export const Sidebar = ({sideBarWidth, api}) => {

    const token = useSelector((state) => state.user.token)
    const identityId = useSelector(state => state.user.identityId)

    const isManager = token?.profile?.role?.includes('Manager');
    const isUser = token?.profile?.role?.includes('User');

    const isDomainsMenuActive = useSelector((state) => state.sidebar.isDomainsMenuActive)
    const currentDomainAccount = useSelector((state) => state.domainAccount.currentAccount)

    const dispatch = useDispatch()

    const [domainAccounts, setDomainAccounts] = useState([])

    useDomainAccounts(identityId, async () => await api.getDomainAccounts(token), setDomainAccounts)

    return (
        <nav id="sidebarMenu" className={style.sidebar} style={{marginLeft: sideBarWidth-250, marginRight: 250 - sideBarWidth, width: 250}}>
            
            <Link to="/" className="nav-link ">                
                <h1 className="nav-item"><i className="bi bi-vimeo text-primary"></i>OU</h1>
            </Link>

            <div>
                <hr/>

                {
                    isManager ?
                        <>
                            <ul className="nav nav-pills flex-column mb-auto">
                                <li className="nav-item mx-2">
                                    <Link to="/domainOffers" className="nav-link">
                                        <i className="bi bi-card-list me-4"></i>
                                        Offers
                                    </Link>
                                </li>
                            </ul> 

                            <hr/>
                        </> : 
                        <></>
                }

                {
                    isUser ?
                        <>
                            <ul className="nav nav-pills flex-column mb-auto">
                                <li className="nav-item mx-2">
                                    <Link to="/newDomain" className="nav-link">
                                        <i className="bi bi-card-list me-4"></i>
                                        New domain
                                    </Link>
                                </li>
                                <li className="nav-item mx-2">
                                    <Link to="/domains" className="nav-link">
                                        <i className="bi bi-card-list me-4"></i>
                                        Domains
                                    </Link>
                                </li>
                            </ul> 

                            <hr/>
                        </> : 
                        <></>
                }
                
                <div style={{marginLeft:18, marginRight:15, marginBottom:15}}>
                    <select onChange={event => dispatch(domainAccountSelected(domainAccounts[event.target.value]))} className="form-select" aria-label="Default select example" >
                        {
                            domainAccounts.map((subscription, i) => <option key={i} value={i}>{subscription.domainName}</option>)
                        }
                    </select>
                </div>
                
                
                <ul className="nav nav-pills flex-column mb-auto">
                    {
                        currentDomainAccount?.isAdmin || currentDomainAccount?.isOwner ?
                            <li className="nav-item mx-2">
                                <Link to="/domainMembers" className="nav-link">
                                    <i className="bi bi-card-text me-4"></i>
                                    Members
                                </Link>
                            </li> : <></>
                    }
                    {
                        

                        currentDomainAccount?.isIssuer ?
                            <li className="nav-item mx-2">
                                <Link to="/issuerBoard" className="nav-link">
                                    <i className="bi bi-card-text me-4"></i>
                                    Issuer board
                                </Link>
                            </li> : <></>
                    }                  
                    {
                        currentDomainAccount ?
                            <li className="nav-item mx-2">
                                <Link to="/holderBoard" className="nav-link">
                                    <i className="bi bi-card-text me-4"></i>
                                    Holder board
                                </Link>
                            </li> : <></>
                    }
                </ul>
                <hr/>               
            </div>
        </nav>
    )
} 

const useDomainAccounts = (identityId, getDomainAccounts, setDomainAccounts) => {

    const dispatch = useDispatch()

    useEffect(() => {
        const effect = async () => {
            if(identityId) {
                const result = await getDomainAccounts() 
                if(result)
                    dispatch(domainAccountSelected(result[0]))
                else 
                    result = []

                setDomainAccounts(result)
            }
        }

        effect()
    },[]);
}