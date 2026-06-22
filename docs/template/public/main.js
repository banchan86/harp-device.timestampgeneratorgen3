import WorkflowContainer from "./workflow.js"

export default {
    defaultTheme: 'light',
    iconLinks: [{
        icon: 'github',
        href: 'https://github.com/harp-tech/device.timestampgeneratorgen3',
        title: 'GitHub'
    }],
    start: () => {
        WorkflowContainer.init();
    }
}
