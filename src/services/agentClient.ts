import { fileSystem } from '@unmango/safir-agent-client/dist/clients';

const baseUrl = process.env.AGENT_URL ?? '';

const {
    list,
    listAsync,
} = fileSystem.createClient(baseUrl);

export {
    list,
    listAsync,
};
