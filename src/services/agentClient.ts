import { fileSystem } from '@unmango/safir-agent-client/dist/clients';

const baseUrl = process.env.REACT_APP_AGENT_URL ?? '';

export const {
    list,
    listAsync,
} = fileSystem.createClient(baseUrl);
